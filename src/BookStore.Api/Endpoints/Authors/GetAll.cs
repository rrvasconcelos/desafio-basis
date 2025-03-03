using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Authors.GetAll;
using MediatR;

namespace BookStore.Api.Endpoints.Authors;

public sealed class GetAll: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("authors", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetAllAuthorsQuery();

                var result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Authors);
    }
}