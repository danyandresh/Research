import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkSizeComponent } from './network-size.component';

describe('NetworkSizeComponent', () => {
  let component: NetworkSizeComponent;
  let fixture: ComponentFixture<NetworkSizeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NetworkSizeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NetworkSizeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
