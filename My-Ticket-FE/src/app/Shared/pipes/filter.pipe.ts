import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(value: any, filterString: string, propName: string[]): any {
    if (value.lenght == 0 || filterString === '') {
      return value;
    }
    let returnArray = [];
    for(const item of value){
      propName.forEach(p => {
        if (item[p].toUpperCase().indexOf(filterString.toUpperCase()) !== -1) {
          returnArray.push(item);
        }
      });
    }
    return returnArray;
  }

}
