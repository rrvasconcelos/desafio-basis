using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Authors.Delete;
using MediatR;

namespace BookStore.Api.Endpoints.Authors;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/authors/{id:int}",
                async (int id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteAuthorCommand(id);

                    var result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                })
            .WithTags(Tags.Authors);
    }
}