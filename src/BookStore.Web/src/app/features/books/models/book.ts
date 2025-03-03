export interface Book {
  id: number;
  title: string;
  publisher: string;
  edition: number;
  publicationYear: string;
  authorIds: number[];
  bookPrices: BookPrice[];
  subjectsId?: number[]; 
  createdAt?: string;
  active?: boolean;
}

export interface BookPrice {
  value?: number; 
  price?: number; 
  purchaseType: PurchaseType;
}

export interface BookRequest {
  id?: number;
  title: string;
  publisher: string;
  edition: number;
  publicationYear: string;
  prices: Price[]; 
  authorsId: number[]; 
  subjectsId: number[]; 
}

export interface Price {
  price: number; 
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
