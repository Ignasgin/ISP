using ISP.Data;
using Microsoft.EntityFrameworkCore;

namespace ISP.Pages;

public class CarSimilarity
{
    private readonly AppDbContext _context;

    public CarSimilarity(AppDbContext context)
    {
        _context = context;
    }

    // Function to find the top 3 similar cars to the given car
    public async Task<List<(int SimilarCarId, double Similarity)>> FindTop3SimilarCarsAsync(int targetCarId)
    {
        // Fetch all cars from the database
        var cars = await _context.Automobilis.ToListAsync();

        if (cars == null || cars.Count < 2)
        {
            return new List<(int, double)>();
        }

        // Find the target car
        var targetCar = cars.FirstOrDefault(c => c.Id_Automobilis == targetCarId);
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
            MaxKaina = cars.Max(c => c.Kaina)
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

        // Calculate similarity with all other cars
        var similarities = new List<(int SimilarCarId, double Similarity)>();

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
                similarities.Add((car.Id_Automobilis, similarity));
            }
        }

        // Return the top 3 most similar cars
        return similarities
            .OrderByDescending(s => s.Similarity)
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
