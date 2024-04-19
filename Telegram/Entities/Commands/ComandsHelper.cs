namespace Telegramchik.Commands;

public static class TelegramHelper
{
	private static int Rnd_Index;
	public static int Get(int max, int min = 0)
	{
		int temp_index = new Random().Next(min, max);
		Rnd_Index = Rnd_Index == temp_index ? (temp_index + 1) % max - 1 : temp_index;
		return Rnd_Index;
	}
}
