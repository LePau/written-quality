import { Component, Input, OnInit } from '@angular/core';
import { WrittenDocumentQuality } from '../document.model';

@Component({
  selector: 'app-written-document-quality',
  templateUrl: './written-document-quality.component.html',
  styleUrls: ['./written-document-quality.component.scss'],
})
export class WrittenDocumentQualityComponent implements OnInit {

  @Input() quality: WrittenDocumentQuality
  constructor() { }

  ngOnInit() {}

}
