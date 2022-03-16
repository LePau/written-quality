import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { WrittenDocumentQualityComponent } from './written-document-quality.component';

describe('WrittenDocumentQualityComponent', () => {
  let component: WrittenDocumentQualityComponent;
  let fixture: ComponentFixture<WrittenDocumentQualityComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ WrittenDocumentQualityComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(WrittenDocumentQualityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
