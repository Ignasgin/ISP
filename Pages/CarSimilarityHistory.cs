using ISP.Data;
using Microsoft.EntityFrameworkCore;

namespace ISP.Pages;

public class CarSimilarityHistory
{
    private readonly AppDbContext _context;

    public CarSimilarityHistory(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<(int SimilarCarId, double WeightedSimilarity)>> FindTop3SimilarCarsAsync(int targetCarId,
        int userId)
    {
        // Fetch user and their watched cars
        var user = await _context.Naudotojas.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id_Naudotojas == userId);

        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        var watchedCars = await _context.Perziuretas_Automobilis.AsQueryable()
            .Where(x => x.Fk_Naudotojas_Id_Naudotojas == userId)
            .ToListAsync();

        var watchedCarIds = watchedCars.Select(x => x.Fk_Automobilis_Id_Automobilis).ToList();

        var cars = await _context.Automobilis.AsQueryable()
            .Where(c => watchedCarIds.Contains(c.Id_Automobilis))
            .ToListAsync();

        var userRatings = await _context.Atsiliepimas.AsQueryable()
            .Where(x => x.Fk_Naudotojas_Id_Naudotojas == userId)
            .ToDictionaryAsync(x => x.Fk_Automobilis_Id_Automobilis, x => x.Ivertinimas);

        if (cars.Count < 2)
        {
            return new List<(int, double)>();
        }

        // Find the target car
        var targetCar = await _context.Automobilis.AsQueryable()
            .FirstOrDefaultAsync(c => c.Id_Automobilis == targetCarId);

        if (targetCar == null)
        {
            throw new ArgumentException("Target car not found.");
        }

        // Get max values for normalization
        var maxValues = new
        {
            MaxMetai = cars.Max(c => c.Metai),
            MaxLitrazas = cars.Max(c => c.Litrazas ?? 0),
            MaxGalia = cars.Max(c => c.Galia),
            MaxRida = cars.Max(c => c.Rida),
            MaxKaina = cars.Max(c => c.Kaina),
            MaxRating = userRatings.Any() ? userRatings.Values.Max() : 5 // Default max rating if no ratings exist
        };

        // Normalize the target car
        var targetVector = new[]
        {
            Normalize(targetCar.Metai, maxValues.MaxMetai),
            Normalize(targetCar.Litrazas ?? 0, maxValues.MaxLitrazas),
            Normalize(targetCar.Galia, maxValues.MaxGalia),
            Normalize(targetCar.Rida, maxValues.MaxRida),
            Normalize((double)targetCar.Kaina, (double)maxValues.MaxKaina)
        };

        // Calculate weighted similarity
        var similarities = new List<(int SimilarCarId, double WeightedSimilarity)>();

        foreach (var car in cars)
        {
            if (car.Id_Automobilis != targetCarId) // Skip the target car
            {
                var carVector = new[]
                {
                    Normalize(car.Metai, maxValues.MaxMetai),
                    Normalize(car.Litrazas ?? 0, maxValues.MaxLitrazas),
                    Normalize(car.Galia, maxValues.MaxGalia),
                    Normalize(car.Rida, maxValues.MaxRida),
                    Normalize((double)car.Kaina, (double)maxValues.MaxKaina)
                };

                var similarity = CosineSimilarity(targetVector, carVector);

                // Adjust similarity with user rating
                var userRating = userRatings.ContainsKey(car.Id_Automobilis) ? userRatings[car.Id_Automobilis] : 0;

                double ratingWeight;
                if (userRating > 0)
                {
                    ratingWeight = userRating < 4
                        ? -1 * (4 - userRating) / 4.0 // Penalize for ratings below 4
                        : (userRating - 4) / (maxValues.MaxRating - 4.0); // Reward for ratings >= 4
                }
                else
                {
                    ratingWeight = 0; // No rating, neutral weight
                }

                var weightedSimilarity = similarity * (1 + ratingWeight); // Adjust similarity with the rating weight
                similarities.Add((car.Id_Automobilis, weightedSimilarity));
            }
        }

        // Return the top 3 most similar cars
        return similarities
            .OrderByDescending(s => s.WeightedSimilarity)
            .Take(3)
            .ToList();
    }

    private static double Normalize(double value, double max)
    {
        return max > 0 ? value / max : 0; // Normalize by max value
    }

    private static double CosineSimilarity(double[] vec1, double[] vec2)
    {
        double dotProduct = 0;
        double magnitude1 = 0;
        double magnitude2 = 0;

        for (int i = 0; i < vec1.Length; i++)
        {
            dotProduct += vec1[i] * vec2[i];
            magnitude1 += vec1[i] * vec1[i];
            magnitude2 += vec2[i] * vec2[i];
        }

        return dotProduct / (Math.Sqrt(magnitude1) * Math.Sqrt(magnitude2));
    }
}