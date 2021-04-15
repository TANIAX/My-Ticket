import { AbstractControl } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { of } from 'rxjs';
/*That is the custom validator class*/ 
//The validator function always take abstract control as argument, this will contain the input of the form
export function Major(control: AbstractControl) {
    //We first check if the value is null or empty
    if (control && control.value !== null || control.value !== undefined) {
        //TODO: Make it dynamicly
        let minMajorDate = new Date();
        //We store the value of the input into a var
        let Birthdate = new Date(control.value);
        //We substract 216 month at the value with is equals to 18 years
        minMajorDate.setMonth(minMajorDate.getMonth() - 216);
        //We add 1441 minutes with is equals to 1 day and 1 minut 
        //We do that because we want the user is accepted if his birthday is today
        minMajorDate.setMinutes(minMajorDate.getMinutes() + 1441);
        // is is not on age, this will return an object with the key 'isNotMajor' to true, this is basicly returning an error
        if (minMajorDate < Birthdate) {
            return { 'isNotMajor': true };
        }
    }
    //If the user is major we need to return null to avoid returning error
    return null;
}

export function confirmPassword(control: AbstractControl) {

    if (control && control.value !== null || control.value !== undefined) {
        const confirmPassword = control.value;
        //control.root allows us to acces to all of the user input of the form
        const passwordControl = control.root.get('Password');

        if (passwordControl) {
            const passwordValue = passwordControl.value;
            if (confirmPassword != passwordValue) {
                return { 'isError': true };
            }
        }
    }
    return null;
}

export function forbiddenNames(control: AbstractControl): { [s: string]: boolean } {
    if (control && control.value !== null || control.value !== undefined) {
        let forbiddenUsernames = ['admin', 'test'];

        if (forbiddenUsernames.indexOf(control.value) != -1) {
            return { 'nameIsForbidden': true };
        }
    }
    return null;
}
export function forbiddenMailDomain(control: AbstractControl): { [s: string]: boolean } {
    if (control && control.value !== null || control.value !== undefined) {
        //TODO Make it dynamic
        const forbiddenMailDomain = ['@4simpleemail.com'];
        const email = control.value;
        //We set a boolean because returning an error at the first forbidden mail domain founded in the loop does not work
        let error = false;
        for(var i=0;i<forbiddenMailDomain.length;i++){
            if(email != null){
                if(email.includes(forbiddenMailDomain[i])) {
                    error=true;
                    //Get out of the loop if one is founded
                    break;
                }
            }
        }
        if (error) {
            return { 'EmailDomainIsForbidden': true };
        }
    }
    return null;
}