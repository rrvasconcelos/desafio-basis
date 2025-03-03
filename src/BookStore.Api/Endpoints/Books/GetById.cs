using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Books.GetById;
using MediatR;

namespace BookStore.Api.Endpoints.Books;

public sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("books/{id:int}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetBookByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Books);
    }
}