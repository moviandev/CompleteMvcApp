using DevIO.Business.Models;
using FluentValidation;
using FluentValidation.Results;

namespace DevIO.Business.Services
{
    public abstract class BaseService
	{
        protected void Notificar(string message)
        {
            // propagar erro até camada de apresentacao
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach(var error in validationResult.Errors)
                Notificar(error.ErrorMessage);
        }

        protected bool RunValidation<TV, TE>(TV validation, TE entity)
            where TV : AbstractValidator<TE>
            where TE : Entity
        {
            var validator = validation.Validate(entity);

            if (validator.IsValid)
                return true;

            Notificar(validator);

            return false;
        }
    }
}
