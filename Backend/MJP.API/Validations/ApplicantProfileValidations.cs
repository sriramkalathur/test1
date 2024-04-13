using System;
using Microsoft.AspNetCore.Mvc;
using MJP.API.Common;
using MJP.Entities;
using MJP.Entities.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MJP.API.Validations
{
    public static class ApplicantProfileValidations
    {

        private static void AddRequiredValidation(this List<ValidationError> errors, string value, string fieldName, string errorMessage){
            if(string.IsNullOrEmpty(value)) {
                errors.Add(new ValidationError(){
                    ErrorMessage = errorMessage,
                    FieldName = fieldName
                });
            }
        }


        private static void AddRequiredValidation(this List<ValidationError> errors, int? value, string fieldName, string errorMessage){
            if(value == null) {
                errors.Add(new ValidationError(){
                    ErrorMessage = errorMessage,
                    FieldName = fieldName
                });
            }
        }



        private static void ValidateAddress(List<ValidationError> errors, Address address, string parentFieldName){
            //In the fieldName concatenate the parentFeildName to get the fill field Name
            AddRequiredValidation(errors, address.Address1, $"{parentFieldName}.Address1", "Address Line1 is required" );
            AddRequiredValidation(errors, address.City, $"{parentFieldName}.City", "City is required" );
            //AddRequiredValidation(errors, address.StateId, $"{parentFieldName}.StateId", "State is required" );
            AddRequiredValidation(errors, address.Pincode, $"{parentFieldName}.Pincode", "Pincode is required" );
        }

        public static ValidationError[] ValidatePersonalInfo(ApplicantPersonalInfo model){

            List<ValidationError> errors = new List<ValidationError>();
            ValidateAddress(errors, model.CurrentAddress, "CurrentAddress");
           
            if(!model.PermanentAddressSameAsCurrent){
                //Permanent address is not same as current. So validate 
                ValidateAddress(errors, model.PermanentAddress, "PermanentAddress");
            }

            if((model.DateOfBirth != null) && DateTime.Now.Subtract(model.DateOfBirth.Value).Days <= 0)
            {
                //Future date
                errors.Add(new ValidationError(){
                    ErrorMessage ="Date of Birth cannot be a future date",
                    FieldName= "DateOfBirth"
                });
            }
           return errors.ToArray();
        }
    }
}
