-- ============================================================================
-- Create table: ys-addresses-regions
-- Schema: public (develop.ys.spaces.postgis / railway)
-- Description: Administrative regions (states, provinces, etc.) per country
--              for address form population and reference lookups.
--              region_type values will map to a forthcoming ys-addresses-region-types table.
-- ============================================================================

CREATE TABLE IF NOT EXISTS public."ys-addresses-regions" (
    id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    country_code      CHAR(2) NOT NULL,
    name              VARCHAR(256) NOT NULL,
    display_name      VARCHAR(256) NOT NULL,
    description       TEXT,
    region_type       VARCHAR(100) NOT NULL,
    is_active         BOOLEAN NOT NULL DEFAULT TRUE,
    meta_data         JSONB,
    created_by        UUID,
    updated_by        UUID,
    created_on        TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_on        TIMESTAMPTZ
);

-- Primary lookup index: all regions for a given country
CREATE INDEX IF NOT EXISTS idx_regions_country_code ON public."ys-addresses-regions" (country_code);

-- Composite index for filtering by country + type
CREATE INDEX IF NOT EXISTS idx_regions_country_type ON public."ys-addresses-regions" (country_code, region_type);

-- Active filter
CREATE INDEX IF NOT EXISTS idx_regions_active ON public."ys-addresses-regions" (is_active);

-- ============================================================================
-- Seed: United States — 50 States
-- ============================================================================

INSERT INTO public."ys-addresses-regions"
    (country_code, name, display_name, description, region_type)
VALUES
    ('US', 'AL', 'Alabama', 'State of Alabama', 'State'),
    ('US', 'AK', 'Alaska', 'State of Alaska', 'State'),
    ('US', 'AZ', 'Arizona', 'State of Arizona', 'State'),
    ('US', 'AR', 'Arkansas', 'State of Arkansas', 'State'),
    ('US', 'CA', 'California', 'State of California', 'State'),
    ('US', 'CO', 'Colorado', 'State of Colorado', 'State'),
    ('US', 'CT', 'Connecticut', 'State of Connecticut', 'State'),
    ('US', 'DE', 'Delaware', 'State of Delaware', 'State'),
    ('US', 'FL', 'Florida', 'State of Florida', 'State'),
    ('US', 'GA', 'Georgia', 'State of Georgia', 'State'),
    ('US', 'HI', 'Hawaii', 'State of Hawaii', 'State'),
    ('US', 'ID', 'Idaho', 'State of Idaho', 'State'),
    ('US', 'IL', 'Illinois', 'State of Illinois', 'State'),
    ('US', 'IN', 'Indiana', 'State of Indiana', 'State'),
    ('US', 'IA', 'Iowa', 'State of Iowa', 'State'),
    ('US', 'KS', 'Kansas', 'State of Kansas', 'State'),
    ('US', 'KY', 'Kentucky', 'Commonwealth of Kentucky', 'State'),
    ('US', 'LA', 'Louisiana', 'State of Louisiana', 'State'),
    ('US', 'ME', 'Maine', 'State of Maine', 'State'),
    ('US', 'MD', 'Maryland', 'State of Maryland', 'State'),
    ('US', 'MA', 'Massachusetts', 'Commonwealth of Massachusetts', 'State'),
    ('US', 'MI', 'Michigan', 'State of Michigan', 'State'),
    ('US', 'MN', 'Minnesota', 'State of Minnesota', 'State'),
    ('US', 'MS', 'Mississippi', 'State of Mississippi', 'State'),
    ('US', 'MO', 'Missouri', 'State of Missouri', 'State'),
    ('US', 'MT', 'Montana', 'State of Montana', 'State'),
    ('US', 'NE', 'Nebraska', 'State of Nebraska', 'State'),
    ('US', 'NV', 'Nevada', 'State of Nevada', 'State'),
    ('US', 'NH', 'New Hampshire', 'State of New Hampshire', 'State'),
    ('US', 'NJ', 'New Jersey', 'State of New Jersey', 'State'),
    ('US', 'NM', 'New Mexico', 'State of New Mexico', 'State'),
    ('US', 'NY', 'New York', 'State of New York', 'State'),
    ('US', 'NC', 'North Carolina', 'State of North Carolina', 'State'),
    ('US', 'ND', 'North Dakota', 'State of North Dakota', 'State'),
    ('US', 'OH', 'Ohio', 'State of Ohio', 'State'),
    ('US', 'OK', 'Oklahoma', 'State of Oklahoma', 'State'),
    ('US', 'OR', 'Oregon', 'State of Oregon', 'State'),
    ('US', 'PA', 'Pennsylvania', 'Commonwealth of Pennsylvania', 'State'),
    ('US', 'RI', 'Rhode Island', 'State of Rhode Island', 'State'),
    ('US', 'SC', 'South Carolina', 'State of South Carolina', 'State'),
    ('US', 'SD', 'South Dakota', 'State of South Dakota', 'State'),
    ('US', 'TN', 'Tennessee', 'State of Tennessee', 'State'),
    ('US', 'TX', 'Texas', 'State of Texas', 'State'),
    ('US', 'UT', 'Utah', 'State of Utah', 'State'),
    ('US', 'VT', 'Vermont', 'State of Vermont', 'State'),
    ('US', 'VA', 'Virginia', 'Commonwealth of Virginia', 'State'),
    ('US', 'WA', 'Washington', 'State of Washington', 'State'),
    ('US', 'WV', 'West Virginia', 'State of West Virginia', 'State'),
    ('US', 'WI', 'Wisconsin', 'State of Wisconsin', 'State'),
    ('US', 'WY', 'Wyoming', 'State of Wyoming', 'State'),

-- ============================================================================
-- Seed: United States — District
-- ============================================================================

    ('US', 'DC', 'District of Columbia', 'District of Columbia', 'District'),

-- ============================================================================
-- Seed: United States — Territories
-- ============================================================================

    ('US', 'AS', 'American Samoa', 'Territory of American Samoa', 'Territory'),
    ('US', 'GU', 'Guam', 'Territory of Guam', 'Territory'),
    ('US', 'MP', 'Northern Mariana Islands', 'Commonwealth of the Northern Mariana Islands', 'Territory'),
    ('US', 'PR', 'Puerto Rico', 'Commonwealth of Puerto Rico', 'Territory'),
    ('US', 'VI', 'U.S. Virgin Islands', 'Territory of the U.S. Virgin Islands', 'Territory');
