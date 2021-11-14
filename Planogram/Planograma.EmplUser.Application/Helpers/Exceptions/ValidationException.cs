using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Planograma.EmplUser.Application.Helpers.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(string message) : base(message) {
            Errors = new Dictionary<string, string[]>()
            {
                {
                    "Validation", new string[] {message}
                }
            };
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}