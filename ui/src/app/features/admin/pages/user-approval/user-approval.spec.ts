import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserApprovalComponent } from './user-approval';

describe('UserApproval', () => {
  let component: UserApprovalComponent;
  let fixture: ComponentFixture<UserApprovalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserApprovalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserApprovalComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
