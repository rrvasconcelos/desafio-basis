using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Books;
using BookStore.Application.Books.Common;
using BookStore.Application.Books.Register;
using BookStore.Domain.Publishing;
using MediatR;

namespace BookStore.Api.Endpoints.Books;

public sealed class Register : IEndpoint
{
    private sealed record Request(
        string Title,
        string Publisher,
        int Edition,
        string PublicationYear,
        List<PricesDto> Prices,
        int[] SubjectsId,
        int[] AuthorsId
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("books", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new RegisterBookCommand(
                    request.Title,
                    request.Publisher,
                    request.Edition,
                    request.PublicationYear,
                    request.Prices,
                    request.SubjectsId,
                    request.AuthorsId
                );

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Books);
    }
}