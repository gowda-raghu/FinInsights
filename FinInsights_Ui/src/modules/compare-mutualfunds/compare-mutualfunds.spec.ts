import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompareMutualfunds } from './compare-mutualfunds';

describe('CompareMutualfunds', () => {
  let component: CompareMutualfunds;
  let fixture: ComponentFixture<CompareMutualfunds>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompareMutualfunds],
    }).compileComponents();

    fixture = TestBed.createComponent(CompareMutualfunds);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
