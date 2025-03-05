namespace VkPostAnalyzer.Domain.Models
{
	public class LetterCount
	{
		public long Id { get; set; }
		public char Letter { get; set; }
		public int Count { get; set; }

		public LetterCount(char letter, int count)
		{
			Letter = letter;
			Count = count;
		}
	}
}

