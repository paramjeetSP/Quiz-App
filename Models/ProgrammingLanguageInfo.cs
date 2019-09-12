using System.Collections.Generic;

namespace QuizApps.Models
{
    public class ProgrammingLanguageInfo
    {
        public int id { get; set; }
        public string language { get; set; }
        public string sampleProgram { get; set; }
        public string slug { get; set; }

        public List<ProgrammingLanguageInfo> GetAllProgrammingLanguagesInfo()
        {
            List<ProgrammingLanguageInfo> allLanguages = new List<ProgrammingLanguageInfo>();
            ProgrammingLanguageInfo lang = new ProgrammingLanguageInfo();

            lang.language = "C#";
            lang.sampleProgram = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(""Hello, world!"");
        }
    }
}";
            lang.id = 1;
            lang.slug = "c#";
            allLanguages.Add(lang);
            lang = new ProgrammingLanguageInfo();
            lang.language = "Javascript";
            lang.sampleProgram = @"print(""Hello, world!"")";
            lang.id = 17;
            lang.slug = "javascript";
            allLanguages.Add(lang);
            lang = new ProgrammingLanguageInfo();
            lang.language = "NodeJS";
            lang.sampleProgram = @"console.log(""Hello, World!"");";
            lang.id = 23;
            lang.slug = "nodejs";
            allLanguages.Add(lang);
            lang = new ProgrammingLanguageInfo();
            lang.language = "Php";
            lang.sampleProgram = @"<?php

    echo ""Hello, world!""

?> ";
            lang.id = 8;
            lang.slug = "php";
            allLanguages.Add(lang);
            lang = new ProgrammingLanguageInfo();
            lang.language = "Swift";
            lang.sampleProgram = @"print(""Hello, world!"")";
            lang.id = 37;
            lang.slug = "swift";
            allLanguages.Add(lang);
            lang = new ProgrammingLanguageInfo();
            lang.language = "C++";
            lang.sampleProgram = @"#include <iostream>

int main()
{
    std::cout << ""Hello, world!\n"";
}
            ";
            lang.slug = "cpp";
            lang.id = 27;
            allLanguages.Add(lang);
            lang = new ProgrammingLanguageInfo();
            lang.language = "Java";
            lang.sampleProgram = @"import java.util.*;
import java.lang.*;

class Rextester
{  
    public static void main(String args[])
    {
        System.out.println(""Hello, World!"");
    }
        }";
            lang.slug = "java";
            lang.id = 4;
            allLanguages.Add(lang);
            return allLanguages;
        }
    }
}