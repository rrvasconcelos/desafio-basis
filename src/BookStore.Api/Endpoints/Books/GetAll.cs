using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Books.GetAll;
using MediatR;

namespace BookStore.Api.Endpoints.Books;

public sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("books", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetAllBooksQuery();

                var result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Books);
    }
}