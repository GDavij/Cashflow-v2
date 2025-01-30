import { Component, Inject } from '@angular/core';
import { Subject } from 'rxjs';
import {Dialog, DIALOG_DATA, DialogRef} from '@angular/cdk/dialog';
import { ButtonComponent } from "../../button/button.component";

@Component({
  selector: 'app-delete',
  imports: [ButtonComponent],
  templateUrl: './delete.component.html',
  styleUrl: './delete.component.scss'
})
export class DeleteComponent {

  message: string = "Are you sure you want to delete this item?";

  constructor(@Inject(DIALOG_DATA) message: string, private readonly dialogRef: DialogRef) {
    this.message = message ? message : this.message;
   }


   answer(confirmed: boolean) {
    this.dialogRef.close(confirmed);
   }
}
