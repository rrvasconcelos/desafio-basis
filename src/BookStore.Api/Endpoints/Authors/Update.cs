using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Authors.Update;
using MediatR;

namespace BookStore.Api.Endpoints.Authors;

public class Update : IEndpoint
{
    private sealed record Request(int Id, string Name);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("authors/{id:int}", async (int id, Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("Id does not match");
                }
                
                var command = new UpdateAuthorCommand(request.Id, request.Name);

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Authors);
    }
}