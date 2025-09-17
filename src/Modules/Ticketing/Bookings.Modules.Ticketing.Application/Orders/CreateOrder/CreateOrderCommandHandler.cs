using System.Data.Common;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Application.Abstractions.Data;
using Bookings.Modules.Ticketing.Application.Abstractions.Payments;
using Bookings.Modules.Ticketing.Application.Carts;
using Bookings.Modules.Ticketing.Domain.Customers;
using Bookings.Modules.Ticketing.Domain.Events;
using Bookings.Modules.Ticketing.Domain.Orders;
using Bookings.Modules.Ticketing.Domain.Payment;
using FluentValidation;

namespace Bookings.Modules.Ticketing.Application.Orders.CreateOrder;

public sealed record CreateOrderCommand(Guid CustomerId) : ICommand;

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
    }
}

internal sealed class CreateOrderCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    CartService cartService,
    ITicketTypeRepository ticketTypeRepository,
    IOrderRepository orderRepository,
    IPaymentService paymentService,
    IPaymentRepository paymentRepository) : ICommandHandler<CreateOrderCommand>
{
    public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        await using DbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        Customer? customer = await customerRepository.GetAsync(command.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        }

        var order = Order.Create(customer);

        Cart cart = await cartService.GetAsync(customer.Id, cancellationToken);

        if (!cart.Items.Any())
        {
            return Result.Failure(CartErrors.Empty);
        }

        foreach (CartItem cartItem in cart.Items)
        {
            // This acquires a pessimistic lock or throws an exception if already locked.
            TicketType? ticketType = await ticketTypeRepository.GetWithLockAsync(
                cartItem.TicketTypeId, cancellationToken);

            if (ticketType is null)
            {
                return Result.Failure(TicketTypeErrors.NotFound(cartItem.TicketTypeId));
            }

            Result result = ticketType.UpdateQuantity(cartItem.Quantity);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            order.AddItem(ticketType, cartItem.Quantity, ticketType.Price, ticketType.Currency);
        }

        orderRepository.Insert(order);

        // We're faking a payment gateway request here...
        PaymentResponse paymentResponse = await paymentService.ChargeAsync(order.TotalPrice, order.Currency);

        var paymment = Payment.Create(
            order,
            paymentResponse.TransactionId,
            paymentResponse.Amount,
            paymentResponse.Currency);

        paymentRepository.Insert(paymment);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        await cartService.ClearAsync(customer.Id, cancellationToken);

        return Result.Success();
    }
}
