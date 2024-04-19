using System;
using System.Collections.Generic;


/// <summary>
/// Class for executing the below functionalities
/// </summary>
class Program
{
    /// <summary>
    /// Main function
    /// </summary>
    static void Main(string[] args)
    {
        //Example input from our assignment: 1,2,3,4,1,2,5,1,2,3,4,5
        Console.WriteLine("Enter page reference string (Separate with commas and no spaces):");
        string input = Console.ReadLine();

        //Checks if page is empty
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Error: Page reference is empty!");
            return;
        }


        //Example input: 4
        Console.WriteLine("Enter number of frames: ");
        int frames = Convert.ToInt32(Console.ReadLine());


        //Object for page replacement
        var simulator = new PageReplacement(input, frames);

        //Execute functions from the page replacement class
        simulator.FIFO();
        simulator.LRU();
        simulator.Optimal();
    }
}


/// <summary>
/// This class will deal with using FIFO, Optimal, and thge LRU algotithms to execute page replacements and see how many faults occur
/// </summary>
class PageReplacement
{
    //This array will store the pages
    private int[] pageReferenceString;

    //This will store the numer of frames
    private int frameCount;

    /// <summary>
    /// Function(constructor) that will initialize the pages and the number of frames
    /// </summary>
    public PageReplacement(string input, int frames)
    {

        //Will spit the inputted string by comma and convert it
        pageReferenceString = input.Split(',').Select(int.Parse).ToArray();

        //The number of frames
        frameCount = frames;
    }

    /// <summary>
    /// FIFO Algorithm
    /// </summary>
    public void FIFO()
    {
        //Prints what algorithm is being shown: FIFO
        Console.WriteLine("For FIFO Algorithm:");
        //Thgis list will store the frames
        var frames = new List<int>();
        //We set faults to zero. It will act as a counter
        int faultsCounter = 0;

        //We will use a queue to track what page we are on
        Queue<int> indexQueuePageTracker = new Queue<int>();

        //We will loop and iterate through each referenecd string
        foreach (var page in pageReferenceString)
        {
            //Current page
            //int page = pageReferenceString[i];
            // If the page is not within thge frames
            //if (!frames.Contains(page))
            //{
            //    //Frames are full
            //    if (frames.Count >= frameCount)
            //    {
            //        //We will remove the oldest page
            //        frames.Remove(indexQueuePageTracker.Dequeue());
            //    }
            //    //Add the current page
            //    frames.Add(page);

            //    //Adds to the queue
            //    indexQueuePageTracker.Enqueue(page);
            //    //We shall increment the faults
            //    faultsCounter++;
            //    //Print the current stats
            //    PrintStatus(i + 1, "FIFO", frames, page, faultsCounter);
            //}
            if (!frames.Contains(page))
            {
                if (frames.Count >= frameCount)
                {
                    frames.Remove(indexQueuePageTracker.Dequeue());
                }
                
                frames.Add(page);
                indexQueuePageTracker.Enqueue(page);
                faultsCounter++;

                
                PrintStatus(indexQueuePageTracker.Count, "FIFO", frames,page,faultsCounter);
            }
        }
        //Output the number of faults
        Console.WriteLine($" Total Page Faults: {faultsCounter}\n");
    }

    /// <summary>
    /// Function for the Optimal algorithm
    /// </summary>
    public void Optimal()
    {
        //print the algorithm again
        Console.WriteLine("For Optimal Algorithm:");
        //Again list for storing frames
        var frames = new List<int>();
        //Count the faults
        int faultsCounter = 0;

        //This loop will iterate through each page
        for (int i = 0; i < pageReferenceString.Length; i++)
        {
            //The current page
            int page = pageReferenceString[i];
            //If the page does not have a fame
            if (!frames.Contains(page))
            {
                //If the frames are full
                if (frames.Count >= frameCount)
                {
                    //This array will help determine whihc page in the memory should be replaced when a page fault occurs 
                    //Will also help reduce page faults in Optimal as we will try and replace a page that is least likely to be used
                    int[] optimalEvictFrames = new int[frames.Count];

                    //This will iterate through the frames
                    for (int j = 0; j < frames.Count; j++)
                    {
                        //This will locate the index for the next frame
                        optimalEvictFrames[j] = Array.IndexOf(pageReferenceString, frames[j], i + 1);

                        //If the page is not used again we will then set to the maximum value

                        if (optimalEvictFrames[j] == -1)
                            optimalEvictFrames[j] = int.MaxValue;
                    }

                    //This will remove the frames with maximum use
                    frames.RemoveAt(Array.IndexOf(optimalEvictFrames, optimalEvictFrames.Max()));
                }
                //Add the page to the frame
                frames.Add(page);

                //Incrememnt thge faults counter
                faultsCounter++;
                //Prin
                PrintStatus(i + 1, "Optimal", frames, page, faultsCounter);
            }
        }
        Console.WriteLine($" Total Page Faults: {faultsCounter}\n");
    }


    /// <summary>
    /// This function will do the LRU algorithm
    /// </summary>
    public void LRU()
    {
        //Print out what algorithm it is
        Console.WriteLine("For LRU Algorithm:");
        //Will store the frames
        var frames = new List<int>();
        //Fault counter
        int faultsCounter = 0;

        //Will track the page order
        List<int> pageOrder = new List<int>();

        //We will iterate through each string
        for (int i = 0; i < pageReferenceString.Length; i++)
        {
            //Current page
            int page = pageReferenceString[i];
            //If the page is not within the frame
            if (!frames.Contains(page))
            {
                //If the franes are full
                if (frames.Count >= frameCount)
                {
                    //Remove the least recently used (LRU) algorithm
                    frames.Remove(pageOrder[0]);
                    //Remove the oldest page from the stack
                    pageOrder.RemoveAt(0);
                }
                //Incrememnt fault counter
                faultsCounter++;
            }
            //Else we remove the page frin the stack
            else
            {
                pageOrder.Remove(page);
            }

            //Add the curerent page
            frames.Add(page);
            //Add the current page index to the stack
            pageOrder.Add(page);

            //Print like in the other function
            PrintStatus(i + 1, "LRU", frames, page, faultsCounter);
        }

        //Print the number of faults
        Console.WriteLine($" Total Page Faults: {faultsCounter}\n");
    }

    /// <summary>
    /// This function will print the statuses
    /// </summary>
    private void PrintStatus(int step, string algorithm, List<int> frames, int page, int faults)
    {
        //Output the steps, faults, frames, and the total numer of falts
        Console.WriteLine($" Step {step}: Page fault ({page}) - Frames: [{string.Join(", ", frames)}], Faults: {faults}");
    }
}
  
