import { TestBed } from '@angular/core/testing';

import { StyleAdjusterService } from './style-adjuster.service';

describe('StyleAdjusterService', () => {
  let service: StyleAdjusterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StyleAdjusterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
