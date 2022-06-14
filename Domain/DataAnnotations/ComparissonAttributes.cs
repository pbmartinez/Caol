﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class AttributeGreaterThan : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        public AttributeGreaterThan(string comparisonProperty) { _comparisonProperty = comparisonProperty; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Invalid entry");

            ErrorMessage = ErrorMessageString;

            if (value.GetType() == typeof(IComparable)) throw new ArgumentException("value has not implemented IComparable interface");
            var currentValue = (IComparable)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null) throw new ArgumentException("Comparison property with this name not found");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            if (!ReferenceEquals(value.GetType(), comparisonValue.GetType()))
                throw new ArgumentException("The types of the fields to compare are not the same.");

            return currentValue.CompareTo((IComparable)comparisonValue) > 0 ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class AttributeGreaterThanOrEqual : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        public AttributeGreaterThanOrEqual(string comparisonProperty) { _comparisonProperty = comparisonProperty; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Invalid entry");
            ErrorMessage = ErrorMessageString;

            if (value.GetType() == typeof(IComparable)) throw new ArgumentException("value has not implemented IComparable interface");
            var currentValue = (IComparable)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null) throw new ArgumentException("Comparison property with this name not found");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            if (!ReferenceEquals(value.GetType(), comparisonValue.GetType()))
                throw new ArgumentException("The types of the fields to compare are not the same.");

            return currentValue.CompareTo((IComparable)comparisonValue) >= 0 ? ValidationResult.Success : new ValidationResult(ErrorMessage);

        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class AttributeEqual : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        public AttributeEqual(string comparisonProperty) { _comparisonProperty = comparisonProperty; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Invalid entry");
            ErrorMessage = ErrorMessageString;

            if (value.GetType() == typeof(IComparable)) throw new ArgumentException("value has not implemented IComparable interface");
            var currentValue = (IComparable)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null) throw new ArgumentException("Comparison property with this name not found");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            if (!ReferenceEquals(value.GetType(), comparisonValue.GetType()))
                throw new ArgumentException("The types of the fields to compare are not the same.");

            return currentValue.CompareTo((IComparable)comparisonValue) == 0 ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class AttributeLessThanOrEqual : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        public AttributeLessThanOrEqual(string comparisonProperty) { _comparisonProperty = comparisonProperty; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Invalid entry");
            ErrorMessage = ErrorMessageString;

            if (value.GetType() == typeof(IComparable)) throw new ArgumentException("value has not implemented IComparable interface");
            var currentValue = (IComparable)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null) throw new ArgumentException("Comparison property with this name not found");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            if (!ReferenceEquals(value.GetType(), comparisonValue.GetType()))
                throw new ArgumentException("The types of the fields to compare are not the same.");

            return currentValue.CompareTo((IComparable)comparisonValue) <= 0 ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class AttributeLessThan : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        public AttributeLessThan(string comparisonProperty) { _comparisonProperty = comparisonProperty; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Invalid entry");
            ErrorMessage = ErrorMessageString;

            if (value.GetType() == typeof(IComparable)) throw new ArgumentException("value has not implemented IComparable interface");
            var currentValue = (IComparable)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null) throw new ArgumentException("Comparison property with this name not found");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            if (!ReferenceEquals(value.GetType(), comparisonValue.GetType()))
                throw new ArgumentException("The types of the fields to compare are not the same.");

            return currentValue.CompareTo((IComparable)comparisonValue) < 0 ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}