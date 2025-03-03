using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Books.Common;
using BookStore.Application.Books.Update;
using BookStore.Domain.Publishing;
using MediatR;

namespace BookStore.Api.Endpoints.Books;

public sealed class Update : IEndpoint
{
    private sealed record Request(
        int Id,
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
        app.MapPut("books/{id:int}", async (int id, Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateBookCommand(
                    request.Id,
                    request.Title,
                    request.Publisher,
                    request.Edition,
                    request.PublicationYear,
                    request.Prices,
                    request.SubjectsId,
                    request.AuthorsId
                );

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Books);
    }
}