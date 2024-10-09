import {AbstractControl, FormGroup, ValidationErrors, ValidatorFn} from "@angular/forms";

export function CompareValidation(controlToValidate: string, controlToCompare : string ) : ValidatorFn {
  return (formGroupAsControl: AbstractControl) : ValidationErrors | null => {
    const formGroup = formGroupAsControl as FormGroup;
    const control = formGroup.get(controlToValidate);
    const matchingControl = formGroup.get(controlToCompare);

    if (control?.value != matchingControl?.value) {
      formGroup.get(controlToCompare)?.setErrors({ compareValidator:  {valid: false} });

      return { compareValidator:  {valid: true} };
    }
    else return null;
  }
}
