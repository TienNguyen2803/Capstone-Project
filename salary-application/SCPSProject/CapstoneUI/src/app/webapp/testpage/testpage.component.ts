import { Component, OnInit, TemplateRef, ChangeDetectorRef, Input } from '@angular/core';
import { Select2OptionData } from 'ng-select2';
import { PrimitiveService } from 'src/app/primitive.service';
import { ApiService } from 'src/app/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilityService } from 'src/app/util.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SPCSObject } from 'src/app/class/SPCSObject';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

declare var $: any;

@Component({
  selector: 'app-testpage',
  templateUrl: './testpage.component.html',
  styleUrls: ['./testpage.component.css']
})
export class TestpageComponent implements OnInit {

  Hash: any;

  UserID: any;


  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;

  constructor(private _formBuilder: FormBuilder, private changeDetection: ChangeDetectorRef, private modalService: BsModalService, private validator: PrimitiveService, private api: ApiService, private route: ActivatedRoute, public router: Router, public util: UtilityService) { }

  ngOnInit() {
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
  }
}