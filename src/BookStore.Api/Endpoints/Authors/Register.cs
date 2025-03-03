using BookStore.Api.Extensions;
using BookStore.Api.Infrastructure;
using BookStore.Application.Authors.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Endpoints.Authors;

public sealed class Register: IEndpoint
{
    public sealed record Request(string Name);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("authors", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new RegisterAuthorCommand(request.Name);

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Authors);
    }
}