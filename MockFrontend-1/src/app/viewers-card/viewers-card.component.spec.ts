import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewersCardComponent } from './viewers-card.component';

describe('ViewersCardComponent', () => {
  let component: ViewersCardComponent;
  let fixture: ComponentFixture<ViewersCardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewersCardComponent]
    });
    fixture = TestBed.createComponent(ViewersCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
