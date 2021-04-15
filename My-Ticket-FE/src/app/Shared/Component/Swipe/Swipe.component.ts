import { ChangeDetectionStrategy, Component,OnInit, Input } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';

type PaneType = 1 | 2;

@Component({
  selector: 'app-Swipe',
  templateUrl: './Swipe.component.html',
  styleUrls: ['./Swipe.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    //Dependant the states, the annimation will be different , 'translateX(-33%)' and 'translateX(-66%)' is at this value because there are 3 step, if it was two step it gonna be 0 and 50% 
      trigger('slide', [
        state('1', style({ transform: 'translateX(0)' })),
        state('2', style({ transform: 'translateX(-50%)' })),
        transition('* => *', animate(300))
      ])
    ]
  })
export class SwipeComponent implements OnInit {
  //Default step
  @Input() activePane: PaneType = 1;
  
  constructor() { }

  ngOnInit() {
  }

}