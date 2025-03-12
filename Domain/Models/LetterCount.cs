namespace Domain.Models
{
	public class LetterCount
	{
		public char Letter { get; set; }
		public int Count { get; set; }

		public LetterCount(char letter, int count)
		{
			Letter = letter;
			Count = count;
		}
	}
}

