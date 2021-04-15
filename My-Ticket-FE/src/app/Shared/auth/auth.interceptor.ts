import { HttpInterceptor, HttpRequest, HttpHandler, HttpUserEvent, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { UserService } from "../Service/user.service";
import 'rxjs/add/operator/do';
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (req.headers.get('No-Auth') == "True")
            return next.handle(req.clone());

        if (localStorage.getItem('userToken') != null) {
            const clonedreq = req.clone({
                headers: req.headers.set("Authorization", "Bearer " + localStorage.getItem('userToken'))
            });
            return next.handle(clonedreq)
                .do(
                    succ => { 
                    },
                    err => {
                        if (err.status === 401)
                            this.router.navigate(['User/Login']);
                        else if (err.status === 403)
                            this.router.navigate(['403']);
                        else if (err.status === 404)
                            this.router.navigate(['404']);
                        else if (err.status === 500)
                            this.router.navigate(['500']);
                        else if(err.status === 0){
                            this.router.navigate(['500']);
                        }
                        
                    }
                );
        }
        else {
            this.router.navigate(['User/Login']);;
        }
    }
}
