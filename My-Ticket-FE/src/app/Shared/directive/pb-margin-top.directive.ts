//NOT USED ANYMORE

// //This is call a directive , it will allow to us to set a html tag on element
// import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
// @Directive({
//     //this is the name of the tag element
//     selector: '[pbMarginTop]'
// })
// export class pbMarginTop implements OnInit{
//     //ElementRef is an object witch allows us to obtain the element of the DOM, Renderer2 will allow us to put some style of the element
//     constructor(private elementRef:ElementRef, private renderer:Renderer2){}
//     ngOnInit() {
//         //If the navbar is small ( if the user as scroll the window more than 30px)
//         if (window.scrollY > 30) {
//             //Remove the animation set at navbar.scss
//             this.renderer.setStyle(this.elementRef.nativeElement,'transition','0s')
//             //Set the mat progress bar to under of the header menu
//             this.renderer.setStyle(this.elementRef.nativeElement,'top','46px')
//           } 
        
//     }
    
// }