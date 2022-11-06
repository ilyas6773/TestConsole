namespace Test2Console
{
    class Program
    {
        private static string input;
        private static string[] input1;
        static void Main(string[] args)
        {
            Console.WriteLine("Task 2");
            while (true)
            {
                input = Console.ReadLine();
                input1 = input.Split(' ');
                int[] stat = stats();
                int x = (stat[1]*100 / (stat[1] + stat[2]));

                Console.WriteLine($"Words: {stat[3]}; Spaces: {stat[3]-1}; Upper cases: {stat[0]}; Vowels: {stat[1]}; Consonants: {stat[2]}; Vowels/Letters Ratio {x}%");
                sort();

                foreach (var word in input1)
                {
                    Console.WriteLine(word);
                }
                Console.WriteLine();
            }
        }
        
        static int[] stats()
        {
            int uppercases = 0;

            foreach (var t in input)
            {
                if (t > 64 && t < 91)
                    uppercases++;
            }

            var vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };
            int vows = 0, cons = 0;
            foreach (var t in input)
            {
                if (vowels.Contains(t))
                {
                    vows++;
                }
                else if (t is >= 'a' and <= 'z' or >= 'A' and <= 'Z')
                {
                    cons++;
                }
            }

            int words = input1.Length;

            int[] res = { uppercases, vows, cons, words};
            return res;
        }

        static void sort()
        {
            //for (var i = 0; i < input1.Length; i++)
            //{
            //    char[] characters = input1[i].ToArray();
            //    Array.Sort(characters);
            //    input1[i] = new string(characters);
            //}
            //input1 = input1.OrderBy(x => x).ToArray();

            
            for (var i = 0; i < input1.Length; i++)
            {
                List<char> index = new();
                char[] characters = input1[i].ToArray();
                for (int j = 0; j < characters.Length; j++)
                {
                    if (characters[j] is >= 'A' and <= 'Z')
                    {
                        
                        characters[j] = (char)(characters[j] + 32);
                        index.Add(characters[j]);
                    }
                }
                Array.Sort(characters);

                int y = Array.IndexOf(characters, index[0]);
                Array.Sort(characters);

                foreach (var x in index)
                {
                    characters[Array.IndexOf(characters, x)] = (char)(characters[Array.IndexOf(characters, x)] - 32);
                }

                input1[i] = new string(characters);
            }

            input1 = input1.OrderBy(x => x).ToArray();
        }
    }

    
}