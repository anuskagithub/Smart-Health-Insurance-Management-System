import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsurancePlansComponent } from './plans';

describe('Plans', () => {
  let component: InsurancePlansComponent;
  let fixture: ComponentFixture<InsurancePlansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InsurancePlansComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InsurancePlansComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
