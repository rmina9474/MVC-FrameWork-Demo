import React, { useState } from "react";
import styled from "styled-components";
import { Link } from "react-router-dom";

const OrderNowContainer = styled.div`
  width: 100%;
`;

const HeroSection = styled.section`
  background-color: #d4e9e2;
  padding: 64px 20px;
  text-align: center;
`;

const HeroContent = styled.div`
  max-width: 800px;
  margin: 0 auto;
`;

const HeroTitle = styled.h1`
  font-size: 2.8rem;
  margin-bottom: 24px;
  color: #1e3932;

  @media (max-width: 768px) {
    font-size: 2rem;
  }
`;

const HeroSubtitle = styled.p`
  font-size: 1.5rem;
  margin-bottom: 32px;
  color: #1e3932;

  @media (max-width: 768px) {
    font-size: 1.25rem;
  }
`;

const StoreFinderSection = styled.section`
  padding: 64px 20px;
  max-width: 1200px;
  margin: 0 auto;
`;

const SectionTitle = styled.h2`
  font-size: 2rem;
  margin-bottom: 32px;
  text-align: center;

  @media (max-width: 768px) {
    font-size: 1.8rem;
  }
`;

const StoreSearchContainer = styled.div`
  max-width: 600px;
  margin: 0 auto 48px;
`;

const SearchForm = styled.form`
  display: flex;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
  overflow: hidden;

  @media (max-width: 576px) {
    flex-direction: column;
  }
`;

const SearchInput = styled.input`
  flex: 1;
  padding: 16px;
  border: none;
  font-size: 1rem;
  outline: none;

  &::placeholder {
    color: rgba(0, 0, 0, 0.4);
  }
`;

const SearchButton = styled.button`
  background-color: #00754a;
  color: white;
  border: none;
  padding: 16px 24px;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.3s ease;

  &:hover {
    background-color: #005c3a;
  }

  @media (max-width: 576px) {
    padding: 12px;
  }
`;

const MapContainer = styled.div`
  width: 100%;
  height: 500px;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  margin-bottom: 32px;
  background-color: #f5f5f5;
  position: relative;
`;

const MapPlaceholder = styled.div`
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
  color: rgba(0, 0, 0, 0.5);
`;

const StoreList = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 24px;

  @media (max-width: 576px) {
    grid-template-columns: 1fr;
  }
`;

const StoreCard = styled.div`
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease;

  &:hover {
    transform: translateY(-5px);
    box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
  }
`;

const StoreImage = styled.div`
  height: 180px;
  background-color: #f5f5f5;
  position: relative;

  img {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
`;

const StoreDetails = styled.div`
  padding: 20px;
`;

const StoreName = styled.h3`
  font-size: 1.25rem;
  margin-bottom: 8px;
`;

const StoreAddress = styled.p`
  color: rgba(0, 0, 0, 0.7);
  margin-bottom: 16px;
  line-height: 1.5;
`;

const StoreFeatures = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 16px;
`;

const FeatureTag = styled.span`
  background-color: #f7f7f7;
  color: rgba(0, 0, 0, 0.7);
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 0.875rem;
  display: inline-flex;
  align-items: center;

  svg {
    margin-right: 4px;
  }
`;

const StoreActions = styled.div`
  display: flex;
  justify-content: space-between;
`;

const ActionButton = styled(Link)`
  display: inline-block;
  padding: 8px 16px;
  border-radius: 50px;
  font-weight: 600;
  font-size: 0.875rem;
  text-align: center;
  transition: all 0.3s ease;
`;

const PrimaryButton = styled(ActionButton)`
  background-color: #00754a;
  color: white;

  &:hover {
    background-color: #005c3a;
  }
`;

const SecondaryButton = styled(ActionButton)`
  border: 1px solid #00754a;
  color: #00754a;

  &:hover {
    background-color: rgba(0, 117, 74, 0.1);
  }
`;

const OrderOptionsSection = styled.section`
  padding: 64px 20px;
  background-color: #f7f7f7;
`;

const OptionsGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 32px;
  max-width: 1200px;
  margin: 0 auto;

  @media (max-width: 576px) {
    grid-template-columns: 1fr;
  }
`;

const OptionCard = styled.div`
  background-color: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease;

  &:hover {
    transform: translateY(-5px);
  }
`;

const OptionImage = styled.div`
  height: 200px;

  img {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
`;

const OptionContent = styled.div`
  padding: 24px;
  text-align: center;
`;

const OptionTitle = styled.h3`
  font-size: 1.5rem;
  margin-bottom: 12px;
`;

const OptionDescription = styled.p`
  color: rgba(0, 0, 0, 0.7);
  margin-bottom: 24px;
  line-height: 1.6;
`;

const CTAButton = styled(Link)`
  display: inline-block;
  background-color: #00754a;
  color: white;
  font-weight: 600;
  padding: 8px 24px;
  border-radius: 50px;
  font-size: 1rem;
  transition: all 0.3s ease;

  &:hover {
    background-color: #005c3a;
  }
`;

// Mock data for nearby stores
const nearbyStores = [
  {
    id: 1,
    name: "Starbucks - Downtown",
    address: "123 Main Street, City Center, NY 10001",
    features: ["Mobile Order & Pay", "Drive-Thru"],
    image:
      "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-67628.jpg",
  },
  {
    id: 2,
    name: "Starbucks - Westside Mall",
    address: "456 Shopping Avenue, Westside Mall, NY 10002",
    features: ["Mobile Order & Pay", "Nitro Cold Brew"],
    image:
      "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-67619.jpg",
  },
  {
    id: 3,
    name: "Starbucks - Central Park",
    address: "789 Park Road, Central Park, NY 10003",
    features: ["Mobile Order & Pay", "Outdoor Seating"],
    image:
      "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-67627.jpg",
  },
];

// Mock data for order options
const orderOptions = [
  {
    id: 1,
    title: "Order Ahead for Pickup",
    description:
      "Save time by ordering ahead on the Starbucks® app. Just order, pay, and pick up your favorites without waiting in line.",
    image:
      "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-67671.jpg",
    cta: "Order Now",
    link: "/menu",
  },
  {
    id: 2,
    title: "Get Delivery",
    description:
      "Have your Starbucks favorites delivered to your door. Just use the Starbucks® app or your favorite delivery service.",
    image:
      "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-67672.jpg",
    cta: "Order Delivery",
    link: "/delivery",
  },
  {
    id: 3,
    title: "Drive-Thru",
    description:
      "Grab your favorites without leaving your car. Use the app to find Drive-Thru locations near you.",
    image:
      "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-67663.jpg",
    cta: "Find Drive-Thru",
    link: "/store-locator",
  },
];

function OrderNow() {
  const [searchQuery, setSearchQuery] = useState("");

  const handleSearchChange = (e) => {
    setSearchQuery(e.target.value);
  };

  const handleSearchSubmit = (e) => {
    e.preventDefault();
    // In a real app, this would trigger an API call to find stores
    console.log("Searching for:", searchQuery);
  };

  return (
    <OrderNowContainer>
      <HeroSection>
        <HeroContent>
          <HeroTitle>Order & Pickup</HeroTitle>
          <HeroSubtitle>
            Save time with easy mobile ordering and contactless pickup options.
          </HeroSubtitle>
        </HeroContent>
      </HeroSection>

      <StoreFinderSection>
        <SectionTitle>Find a Store Near You</SectionTitle>
        <StoreSearchContainer>
          <SearchForm onSubmit={handleSearchSubmit}>
            <SearchInput
              type="text"
              placeholder="Enter your address, city, or zip code"
              value={searchQuery}
              onChange={handleSearchChange}
            />
            <SearchButton type="submit">Search</SearchButton>
          </SearchForm>
        </StoreSearchContainer>

        <MapContainer>
          <MapPlaceholder>
            <svg
              width="48"
              height="48"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M12 2C8.13 2 5 5.13 5 9C5 14.25 12 22 12 22C12 22 19 14.25 19 9C19 5.13 15.87 2 12 2ZM12 11.5C10.62 11.5 9.5 10.38 9.5 9C9.5 7.62 10.62 6.5 12 6.5C13.38 6.5 14.5 7.62 14.5 9C14.5 10.38 13.38 11.5 12 11.5Z"
                fill="currentColor"
              />
            </svg>
            <p style={{ marginTop: "12px" }}>
              Search for stores to see the map
            </p>
          </MapPlaceholder>
        </MapContainer>

        <SectionTitle style={{ fontSize: "1.75rem" }}>
          Nearby Stores
        </SectionTitle>
        <StoreList>
          {nearbyStores.map((store) => (
            <StoreCard key={store.id}>
              <StoreImage>
                <img src={store.image} alt={store.name} />
              </StoreImage>
              <StoreDetails>
                <StoreName>{store.name}</StoreName>
                <StoreAddress>{store.address}</StoreAddress>
                <StoreFeatures>
                  {store.features.map((feature, index) => (
                    <FeatureTag key={index}>
                      {feature === "Mobile Order & Pay" ? (
                        <>
                          <svg
                            width="16"
                            height="16"
                            viewBox="0 0 24 24"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <path
                              d="M17 1H7C5.9 1 5 1.9 5 3V21C5 22.1 5.9 23 7 23H17C18.1 23 19 22.1 19 21V3C19 1.9 18.1 1 17 1ZM17 19H7V5H17V19Z"
                              fill="currentColor"
                            />
                            <path
                              d="M12 17C12.83 17 13.5 16.33 13.5 15.5C13.5 14.67 12.83 14 12 14C11.17 14 10.5 14.67 10.5 15.5C10.5 16.33 11.17 17 12 17Z"
                              fill="currentColor"
                            />
                          </svg>
                        </>
                      ) : feature === "Drive-Thru" ? (
                        <>
                          <svg
                            width="16"
                            height="16"
                            viewBox="0 0 24 24"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <path
                              d="M18.92 6.01C18.72 5.42 18.16 5 17.5 5H6.5C5.84 5 5.29 5.42 5.08 6.01L3 12V20C3 20.55 3.45 21 4 21H5C5.55 21 6 20.55 6 20V19H18V20C18 20.55 18.45 21 19 21H20C20.55 21 21 20.55 21 20V12L18.92 6.01ZM6.5 16C5.67 16 5 15.33 5 14.5C5 13.67 5.67 13 6.5 13C7.33 13 8 13.67 8 14.5C8 15.33 7.33 16 6.5 16ZM17.5 16C16.67 16 16 15.33 16 14.5C16 13.67 16.67 13 17.5 13C18.33 13 19 13.67 19 14.5C19 15.33 18.33 16 17.5 16ZM5 11L6.5 6.5H17.5L19 11H5Z"
                              fill="currentColor"
                            />
                          </svg>
                        </>
                      ) : feature === "Nitro Cold Brew" ? (
                        <>
                          <svg
                            width="16"
                            height="16"
                            viewBox="0 0 24 24"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <path
                              d="M7.5 7L5.5 5H18.5L16.5 7H7.5Z"
                              fill="currentColor"
                            />
                            <path
                              d="M16.5 10H7.5L6.5 19H17.5L16.5 10Z"
                              fill="currentColor"
                            />
                          </svg>
                        </>
                      ) : (
                        <>
                          <svg
                            width="16"
                            height="16"
                            viewBox="0 0 24 24"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <path
                              d="M5.5 22V20H3.5V18H5.5V16H7.5V18H9.5V20H7.5V22H5.5ZM11.5 22V20H9.5V18H11.5V16H13.5V18H15.5V20H13.5V22H11.5ZM17.5 22V20H15.5V18H17.5V16H19.5V18H21.5V20H19.5V22H17.5ZM3.5 14V3C3.5 2.45 3.95 2 4.5 2H14.5L20.5 8V14H3.5ZM14.5 8H19.1L14.5 3.4V8Z"
                              fill="currentColor"
                            />
                          </svg>
                        </>
                      )}
                      {feature}
                    </FeatureTag>
                  ))}
                </StoreFeatures>
                <StoreActions>
                  <SecondaryButton to={`/stores/${store.id}`}>
                    Store Details
                  </SecondaryButton>
                  <PrimaryButton to={`/order/${store.id}`}>
                    Order Now
                  </PrimaryButton>
                </StoreActions>
              </StoreDetails>
            </StoreCard>
          ))}
        </StoreList>
      </StoreFinderSection>

      <OrderOptionsSection>
        <SectionTitle>Ways to Order</SectionTitle>
        <OptionsGrid>
          {orderOptions.map((option) => (
            <OptionCard key={option.id}>
              <OptionImage>
                <img src={option.image} alt={option.title} />
              </OptionImage>
              <OptionContent>
                <OptionTitle>{option.title}</OptionTitle>
                <OptionDescription>{option.description}</OptionDescription>
                <CTAButton to={option.link}>{option.cta}</CTAButton>
              </OptionContent>
            </OptionCard>
          ))}
        </OptionsGrid>
      </OrderOptionsSection>
    </OrderNowContainer>
  );
}

export default OrderNow;
