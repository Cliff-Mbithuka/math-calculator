// - You need to create a Math game containing the 4 basic operations
// - The divisions should result on INTEGERS ONLY and dividends should go from 0 to 100. Example: Your app shouldn't present the division 7/2 to the user, since it doesn't result in an integer.
// - Users should be presented with a menu to choose an operation
// - You should record previous games in a List and there should be an option in the menu for the user to visualize a history of previous games.
// - You don't need to record results on a database. Once the program is closed the results will be deleted.

using System.Diagnostics;
using System.Formats.Asn1;
using willCodeMathGame;
using WillCodeMathGame;

MathGameLogic mathGame = new MathGameLogic();
Random random = new Random();

int firstNumber;
int secondNumber;
int userMenuSelection;
int score;
bool gameOver = false;

public enum DifficultyLevel{
    Easy = 45,
    Medium = 30,
    Hard = 15
}

static DifficultyLevel ChangeDifficulty(){
    int userSelection = 0;

    Console.WriteLine("Please select the difficulty level:");
    Console.WriteLine("1 - Easy");
    Console.WriteLine("2 - Medium");
    Console.WriteLine("3 - Hard");

    while(!int.TryParse(Console.ReadLine(), out userSelection) || userSelection < 1 || userSelection > 3)
    {
    Console.WriteLine("Please enter a valid option 1-3.");
    }

    switch(userSelection){
        case 1:
            return DifficultyLevel.Easy;
        case 2:
            return DifficultyLevel.Medium;
        case 3: 
            return DifficultyLevel.Hard;
    }
    return DifficultyLevel.Easy;
}

static void DisplayMathGameQuestion(int firstNumber, int secondNumber, char operation){
    Console.WriteLine($"What is {firstNumber} {operation} {secondNumber} = ??");
}

static int GetUserMenuSelection(MathGameLogic mathGame){
    int selection = -1;
    mathGame.ShowMenu();
    while(selection < 1 || selection > 8){
        while(!int.TryParse(Console.ReadLine(), out selection)){
            Console.WriteLine("Please enter a valid option 1-8.");
    }

    if(!(selection >= 1 && selection <= 8)){
        Console.WriteLine("Please enter a valid option 1-8.");
    }
    }
    return selection;
}

static async Task<int> GetUserResponse(DifficultyLevel difficulty){
   int response = 0;
   int timeout = (int)difficulty;

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    Task<string?> getUserInputTask = Task.Run(() => Console.ReadLine());

    try
    {
        string? result = await Task.WhenAny(getUserInputTask, Task.Delay(timeout * 1000)) == getUserInputTask ? getUserInputTask.Result : null;

        stopwatch.Stop();

        if(result != null && int.TryParse(result, out response)){
            Console.WriteLine($"Time taken to answer: {stopwatch.Elapsed.ToString(@"m\::ss\.fff")}");
            return response;
          
        }else{
            throw new OperationCanceledException();
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Time's up!");
        return -1;
    }
}

static int ValideResult(int result, int? userResponse, int score){
    if(result == userResponse){
        Console.WriteLine("You answered correctly; You earned 5 point!");
        score += 5;
    }else{
        Console.WriteLine("Try again");
       Console.WriteLine($"The correct answer is {result}");
    }
    return score;
}

static async Task<int> PerformOperation(MathGameLogic mathGame, int firstNumber, int secondNumber, int score, char operation, DifficultyLevel difficulty){
    int result;
    int? userResponse;

    DisplayMathGameQuestion(firstNumber, secondNumber, operation);
   result = mathGame.MathOperation(firstNumber, secondNumber, operation);

    userResponse = await GetUserResponse(difficulty);
    score += ValideResult(result, userResponse, score);
    return score;
 
}