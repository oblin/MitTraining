export class RegFile {
  /*[StringLength(10), Required, Key]*/
  public RegNo: string;
  /*[StringLength(20), Required]*/
  public Name: string;
  /*[StringLength(1), Required]*/
  public Sex: string;
  /*[StringLength(10), Required]*/
  public IdNo: string;
  public BirthDate: Date;
}
