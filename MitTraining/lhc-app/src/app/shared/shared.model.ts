export class PagedList<T>
{
  public list: T[];
  public count: number = 0;
  public currentPage: number = 1;
  public pageCount: number = 5
};

export class Dictionary {
  constructor(key: string, value: string) {
    this.key = key;
    this.value = value;
  }

  public key: string;
  public value: string;
}
