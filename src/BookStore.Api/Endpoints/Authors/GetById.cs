using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Authors.GetById;
using MediatR;

namespace BookStore.Api.Endpoints.Authors;

public class GetById: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("authors/{id:int}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetAuthorByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Authors);
    }
}