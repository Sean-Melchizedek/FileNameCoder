// See https://aka.ms/new-console-template for more information
Console.WriteLine("read files:");
string command = args[0];
string dir = args[1];
Console.WriteLine($"from: {dir}");
DirectoryInfo d = new($"{dir}");
FileInfo[] infos = d.GetFiles();
string[] outputs = new string[infos.Length];


switch (command)
{
    case "-encode":
        Console.WriteLine("encode");
        outputs = Encode(infos);
        break;
    case "-decode":
        Console.WriteLine("decode");
        outputs = Decode(infos);
        break;
    default:
        break;
}
Console.WriteLine("good to go?(y/n)");
string? userResponse = Console.ReadLine();
switch (userResponse)
{
    case "y":
    case "Y":
        Console.WriteLine("renaming");
        RenameFiles(infos, outputs);
        break;
    case "n":
    case "N":
        Console.WriteLine("dropped");
        break;
    default:
        break;
}

string[] Encode(FileInfo[] inputs)
{
    string[] outputs = new string[inputs.Length];
    for (int i = 0; i < inputs.Length; i++)
    {
        outputs[i] = Base64Encode(inputs[i].FullName);
        Console.WriteLine(outputs[i]);
    }
    return outputs;
}

string[] Decode(FileInfo[] inputs)
{
    string[] outputs = new string[inputs.Length];
    for (int i = 0; i < inputs.Length; i++)
    {
        outputs[i] = Base64Decode(inputs[i].Name);
        Console.WriteLine(outputs[i]);
    }
    return outputs;
}

// see https://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
// replace "/" to "_"
static string Base64Encode(string plainText)
{
    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
    return System.Convert.ToBase64String(plainTextBytes).Replace("/", "_");
}

static string Base64Decode(string base64EncodedData)
{
    base64EncodedData = base64EncodedData.Replace("_", "/");
    Console.WriteLine($"{base64EncodedData} ready to decode");
    var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
    return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
}

void RenameFiles(FileInfo[] infos, string[] newNames)
{
    for (int i = 0; i < infos.Length; i++)
    {
        Console.WriteLine($"{infos[i].FullName} changed to {newNames[i]}");

        if (infos[i].Exists)
        {
            try
            {
                File.Move(infos[i].FullName, infos[i].FullName.Replace(infos[i].FullName, newNames[i]));
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Console.WriteLine($"FAILED: {infos[i].FullName} changed to {newNames[i]}");
                throw;
            }
        }
        
    }
}