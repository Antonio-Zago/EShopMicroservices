
using BuildingBlocks.CQRS;
using Catalog.Api.Models;
using Marten;

namespace Catalog.Api.Products.CreateProduct
{
    //Record é parecido com classe porém as propriedades são imutáveis, ou seja, só podem ser alteradas via construtor na criação do objeto
    //Como esse record está herdando de IRequest, o MediatR vai entender que isso é um request
    //CreateProductCommand é como se fosse o dto que será recebido pelo controller
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    // Como a classe handler está herdando de IRequestHandler o MediatR entende que essa classe é um dos possíveis classes 
    //com a lógica que os controllers podem chamar
    internal class CreateProductHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            var creatProductResult = new CreateProductResult(product.Id);

            return creatProductResult;
        }
    }
}
