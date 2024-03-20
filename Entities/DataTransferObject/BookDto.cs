namespace Entities.DataTransferObject
{
	public record BookDto
	{
        public int ID { get; init; }
        public string Title { get; init; }
        public decimal Price { get; init; }
    }
}
