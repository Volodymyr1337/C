using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs_game
{
    internal class Node
    {
        public bool used;
        public string Data;
        public Node Next;
    }

    public class ListClass
    {
        private Node head;
        private Node tail;

        public ListClass() // constructor
        {
            head = null;
            tail = null;
        }
        // adding elements
        public void AddNew(string data) 
        {
            if (head == null)  //creating the first list element and this li-element is the tail
            {
                head = new Node(); 
                head.Data = data;
                head.used = false;
                tail = head;
                head.Next = null;
            }
            else
            {
                Node newNode = new Node();
                tail.Next = newNode;
                tail = newNode;
                tail.Data = data;
                tail.used = false;
                tail.Next = null;
            }
        }
        // print answer word
        public string PrintAnswer() 
        {
            Node ptr = new Node();
            for (ptr = head; ptr != null; ptr = ptr.Next)
                if (ptr.Data.Length > 2 && ptr.used == false)
                {
                    ptr.used = true;
                    return ptr.Data;
                }
            return "end";
        }
        // checking equal word in dictionary
        public byte WordChecking(string userInput, byte len) 
        {
            Node ptr = new Node();
            for (ptr = head; ptr != null; ptr = ptr.Next)
            {
                string dictionaryWord = ptr.Data;
                if (dictionaryWord.Length == len)
                {
                    byte i;
                    for (i = 0; i < len; i++)
                    {
                        if (dictionaryWord[i] == userInput[i])
                        {
                            if (i == (len - 1)) // if all letters are equal
                            {
                                
                                if (ptr.used == false)
                                {
                                    ptr.used = true;
                                    return 1;
                                }   
                                else
                                    return 2;
                            }
                            continue;
                        }
                        else
                            break;
                    }
                }
            }
            return 0;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /* writing dictionary in array of linked lists */           
            ListClass[] list = new ListClass[26];
            writingDictionary(list);

            Action(list);
            
            Console.ReadKey();
        }

        static void Action(ListClass[] list)
        {
            byte tries = 4;
            string lastWord = "";
            byte start = 0;
            byte correct = 1;

            while (tries > 0)
            {
                /* --- user input --- */
                string userInput;
                byte len;
                do
                {
                    Console.Write("Write word: ");
                    userInput = Console.ReadLine();
                    len = (byte)userInput.Length;
                    if (len <= 2)
                    {
                        Console.WriteLine("Word must include 2 or more letters!");
                        tries--;
                        correct = 0;
                    }
                    else if (start != 0)
                    {
                        byte lastLet = (byte)(lastWord.Length - 1);
                        // if last letter isn't equal first letter of user input
                        if (lastWord[lastLet] != userInput[0])
                        {
                            Console.WriteLine("incorrect first letter in the word!");
                            tries--;
                            correct = 0;
                        }
                        else
                            correct = 1;
                    }
                }
                while (correct == 0 && tries > 0);
                Console.Clear();
                start = 1;
                if (tries > 0)
                {
                    string userInputComplete = userInput.ToLower(); // update to lower case

                    // checking number of the first letter
                    byte index = CheckLetter(userInputComplete);

                    // checking word in dictionary
                    byte checking = list[index].WordChecking(userInputComplete, len);
                    if (checking == 0)
                    {
                        Console.WriteLine("There is no such word in the dictionary!");
                        tries--;
                    }
                    else if (checking == 2)
                    {
                        Console.WriteLine("This word was used!!");
                        tries--;
                    }
                    else
                    {
                        byte lastLetter = CheckLastLetter(userInputComplete, len);
                        lastWord = list[lastLetter].PrintAnswer();
                    }
                }

                if (tries > 0)
                    Console.WriteLine("Last word: " + lastWord + "\t\t\tYour tries: " + (tries - 1));
                else
                    Console.WriteLine("You lose!");
            }
        }

        static void writingDictionary(ListClass[] list)
        {

            byte k = 0;     // number of the first letter in the word
            byte n = 1;     // number which means is it first li-element
            int count = 0;  // count of words in dictonary
            var file = new StreamReader("dictionary.txt");
            string word;
            while ((word = file.ReadLine()) != null)
            {
                count++;
                k = CheckLetter(word);

                // if k != n - creating first li-element
                if (k != n)
                {
                    list[k] = new ListClass();
                    list[k].AddNew(word);
                    n = k;
                }
                else
                    list[k].AddNew(word);
            }
        }
        // checking number of the first letter in the word 
        public static byte CheckLetter(string word)
        {
            int letter = word[0];
            if (letter >= 65 && letter <= 90)
                letter = letter - 'A';
            else
                letter = letter - 'a';
            return (byte)letter;
        }
        //checking number of the last letter in the word
        static byte CheckLastLetter(string word, byte len)
        {
            int letter = word[len - 1];
            letter = letter - 'a';
            return (byte)letter;
        }
    }
}
