// models/book.ts
export interface Book {
  id: number;
  title: string;
  publisher: string;
  edition: number;
  publicationYear: string;
  authorIds: number[];
  bookPrices: BookPrice[];
  subjectsId?: number[]; // Adicionado para compatibilidade com a API
  createdAt?: string;
  active?: boolean;
}

// Interface para os preços retornados pela API
export interface BookPrice {
  value?: number; // Campo usado pela API
  price?: number; // Campo usado pelo frontend
  purchaseType: PurchaseType;
}

// Interface para a requisição ao backend
export interface BookRequest {
  id?: number;
  title: string;
  publisher: string;
  edition: number;
  publicationYear: string;
  prices: Price[]; // Nome usado na requisição
  authorsId: number[]; // Nome usado na requisição
  subjectsId: number[]; // Nome usado na requisição
}

export interface Price {
  price: number; // Mantemos como number pois é obrigatório para o envio
  purchaseType: PurchaseType;
}

export enum PurchaseType {
  PhysicalStore = 1,
  Online = 2,
  SelfService = 3,
  Event = 4,
  Subscription = 5,
  EBook = 6,
  Audiobook = 7,
  Rental = 8
}





export interface BookFormData {
  id: number;
  title: string;
  publisher: string;
  edition: number;
  publicationYear: string;
  prices: Price[];
  subjects: {id: number, name: string}[];
  authors: {id: number, name: string}[];
}
