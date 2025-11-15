using System.ComponentModel.DataAnnotations;

namespace CargoWiseNetLibrary.Validation;

/// <summary>
/// Model validator for CargoWise models using Data Annotations
/// </summary>
/// <typeparam name="T">The type of model to validate</typeparam>
public class ModelValidator<T> where T : class
{
    /// <summary>
    /// Validates a single model
    /// </summary>
    /// <param name="model">The model to validate</param>
    /// <returns>Validation result</returns>
    public ValidationResult Validate(T model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var result = new ValidationResult { IsValid = true };
        var context = new ValidationContext(model);
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

        if (!Validator.TryValidateObject(model, context, validationResults, true))
        {
            result.IsValid = false;
            foreach (var validationResult in validationResults)
            {
                result.Errors.Add(new ValidationError
                {
                    PropertyName = validationResult.MemberNames.FirstOrDefault() ?? string.Empty,
                    ErrorMessage = validationResult.ErrorMessage ?? string.Empty
                });
            }
        }

        return result;
    }

    /// <summary>
    /// Validates a single model asynchronously
    /// </summary>
    /// <param name="model">The model to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    public Task<ValidationResult> ValidateAsync(T model, CancellationToken cancellationToken = default)
    {
        // Data Annotations validation is synchronous, so we wrap it in a Task
        // Future implementations could add async validation logic here
        return Task.FromResult(Validate(model));
    }

    /// <summary>
    /// Validates multiple models
    /// </summary>
    /// <param name="models">The models to validate</param>
    /// <returns>Dictionary of models and their validation results</returns>
    public Dictionary<T, ValidationResult> ValidateMany(IEnumerable<T> models)
    {
        ArgumentNullException.ThrowIfNull(models);

        var results = new Dictionary<T, ValidationResult>();

        foreach (var model in models)
        {
            if (model != null)
            {
                results[model] = Validate(model);
            }
        }

        return results;
    }

    /// <summary>
    /// Validates multiple models asynchronously
    /// </summary>
    /// <param name="models">The models to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dictionary of models and their validation results</returns>
    public async Task<Dictionary<T, ValidationResult>> ValidateManyAsync(
        IEnumerable<T> models,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(models);

        var results = new Dictionary<T, ValidationResult>();

        foreach (var model in models)
        {
            if (model != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                results[model] = await ValidateAsync(model, cancellationToken);
            }
        }

        return results;
    }

    /// <summary>
    /// Validates all models and returns true only if all are valid
    /// </summary>
    /// <param name="models">The models to validate</param>
    /// <returns>True if all models are valid, false otherwise</returns>
    public bool ValidateAll(IEnumerable<T> models)
    {
        ArgumentNullException.ThrowIfNull(models);

        foreach (var model in models)
        {
            if (model != null)
            {
                var result = Validate(model);
                if (!result.IsValid)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Validates all models asynchronously and returns true only if all are valid
    /// </summary>
    /// <param name="models">The models to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if all models are valid, false otherwise</returns>
    public async Task<bool> ValidateAllAsync(
        IEnumerable<T> models,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(models);

        foreach (var model in models)
        {
            if (model != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await ValidateAsync(model, cancellationToken);
                if (!result.IsValid)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Tries to validate a model, returning false instead of throwing on null
    /// </summary>
    /// <param name="model">The model to validate</param>
    /// <param name="result">The validation result if model is not null</param>
    /// <returns>True if validation was performed, false if model was null</returns>
    public bool TryValidate(T? model, out ValidationResult? result)
    {
        result = null;

        if (model == null)
            return false;

        result = Validate(model);
        return true;
    }

    /// <summary>
    /// Gets all validation errors from a collection of models
    /// </summary>
    /// <param name="models">The models to validate</param>
    /// <returns>List of all validation errors found</returns>
    public List<ValidationError> GetAllErrors(IEnumerable<T> models)
    {
        ArgumentNullException.ThrowIfNull(models);

        var allErrors = new List<ValidationError>();

        foreach (var model in models)
        {
            if (model != null)
            {
                var result = Validate(model);
                allErrors.AddRange(result.Errors);
            }
        }

        return allErrors;
    }

    /// <summary>
    /// Gets the count of invalid models in a collection
    /// </summary>
    /// <param name="models">The models to validate</param>
    /// <returns>Number of invalid models</returns>
    public int GetInvalidCount(IEnumerable<T> models)
    {
        ArgumentNullException.ThrowIfNull(models);

        return models.Count(model => model != null && !Validate(model).IsValid);
    }

    /// <summary>
    /// Filters a collection to only valid models
    /// </summary>
    /// <param name="models">The models to filter</param>
    /// <returns>Enumerable of only valid models</returns>
    public IEnumerable<T> GetValidModels(IEnumerable<T> models)
    {
        ArgumentNullException.ThrowIfNull(models);

        return models.Where(model => model != null && Validate(model).IsValid);
    }

    /// <summary>
    /// Filters a collection to only invalid models
    /// </summary>
    /// <param name="models">The models to filter</param>
    /// <returns>Enumerable of only invalid models</returns>
    public IEnumerable<T> GetInvalidModels(IEnumerable<T> models)
    {
        ArgumentNullException.ThrowIfNull(models);

        return models.Where(model => model != null && !Validate(model).IsValid);
    }
}
