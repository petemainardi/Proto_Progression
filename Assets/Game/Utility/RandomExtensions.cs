using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Utility methods for random selection using the Unity Random class.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public static class RandomExtensions
{
    public static DateTime GetDateRandom(DateTime start, DateTime end)
    {
        int days = Random.Range(0, (end - start).Days);
        return end.AddDays(-days);
    }


    public static List<T> GetNRandom<T>(List<T> items, int numRandom)
    {
        List<T> selectedItems = new List<T>();
        numRandom = Math.Min(numRandom, items.Count);

        for (int i = 0; i < items.Count && numRandom > 0; i++)
        {
            if (Random.Range(0, items.Count - i) < numRandom)
            {
                selectedItems.Add(items[i]);
                numRandom--;
            }
        }

        return selectedItems;
    }

    /**
     * Randomly selects a set number of items from the list, choosing all items before choosing
     * duplicates. Results are not guaranteed to be exactly even if the number to select not evenly
     * divisible by the number of supplied items.
     */
    public static List<T> GetEvenlyNRandom<T>(List<T> items, int numRandom, bool capAtListSize = true)
    {
        List<T> selectedItems = new List<T>();
        if (capAtListSize)
            numRandom = Math.Min(numRandom, items.Count);

        while (selectedItems.Count < numRandom && items.Count() > 0)
        {
            List<T> availableItems = items.ToList();
            while(selectedItems.Count < numRandom && availableItems.Count > 0)
            {
                selectedItems.Add(availableItems.RandomRemove());
            }
        }
        return selectedItems;
    }

    /**
     * Randomly distributes a set number of items among different categories.
     * Results are not guaranteed to be exact if the number of supplied items is less
     * than the sum total of category sizes.
     * 
     * items: The objects being distributed.
     * categoryCounts(T1): The categories amongst which objects are being distributed.
     * categoryCounts(int): How many objects to distribute to this category.
     * 
     * Returns a dictionary of item/category pairs.
     * 
     */
    public static Dictionary<T0, T1> DistributeNRandom<T0, T1>(List<T0> items, Dictionary<T1, int> categoryCounts)
    {
        Dictionary<T0, T1> distributedItems = new Dictionary<T0, T1>();
        Dictionary<T1, int> counts = new Dictionary<T1, int>(categoryCounts);

        int total = counts.Values.Sum();
        List<T0> itemsAdjusted = items.Count > total ? GetNRandom(items, total) : items;

        for (int i = 0; i < itemsAdjusted.Count && counts.Count > 0; i++)
        {
            T1 category = counts.Keys.ElementAt(Random.Range(0, counts.Count));
            distributedItems.Add(itemsAdjusted[i], category);

            if (--counts[category] <= 0)
            {
                counts.Remove(category);
            }
        }

        return distributedItems;
    }


    public static Dictionary<T0, T1> DistributeEvenlyRandom<T0, T1>(List<T0> items, List<T1> categories)
    {
        int quotient = items.Count / categories.Count;
        int remainder = items.Count % categories.Count;

        Dictionary<T1, int> counts = new Dictionary<T1, int>();
        foreach (T1 category in categories)
            counts.Add(category, quotient);
        foreach (T1 category in GetNRandom(categories, remainder))
            counts[category]++;

        return DistributeNRandom(items, counts);
    }
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
