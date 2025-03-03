using BookStore.SharedKernel;

namespace BookStore.Domain.Publishing;

public enum PurchaseType
{
    PhysicalStore = 1,      // Compra em loja física
    Online,             // Compra pela internet
    SelfService,        // Compra em quiosque de autoatendimento
    Event,              // Compra em eventos (feiras, lançamentos, etc.)
    Subscription,       // Compra via assinatura (ex: serviços de leitura por assinatura)
    EBook,              // Compra de livro digital (eBook)
    Audiobook,          // Compra de audiobook
    Rental              // Aluguel do livro
}