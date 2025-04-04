import React from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';

const GiftCardsContainer = styled.div`
  width: 100%;
`;

const HeroSection = styled.section`
  background-color: #f2f0eb;
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
  
  @media (max-width: 768px) {
    font-size: 2rem;
  }
`;

const HeroSubtitle = styled.p`
  font-size: 1.5rem;
  margin-bottom: 32px;
  color: rgba(0, 0, 0, 0.7);
  
  @media (max-width: 768px) {
    font-size: 1.25rem;
  }
`;

const ButtonContainer = styled.div`
  display: flex;
  justify-content: center;
  gap: 16px;
  
  @media (max-width: 480px) {
    flex-direction: column;
    align-items: center;
  }
`;

const PrimaryButton = styled(Link)`
  display: inline-block;
  background-color: #00754a;
  color: white;
  font-weight: 600;
  padding: 7px 16px;
  border-radius: 50px;
  font-size: 1rem;
  transition: all 0.3s ease;
  
  &:hover {
    background-color: rgba(0, 117, 74, 0.9);
    transform: translateY(-2px);
  }
`;

const SecondaryButton = styled(Link)`
  display: inline-block;
  background-color: transparent;
  color: #00754a;
  font-weight: 600;
  padding: 7px 16px;
  border-radius: 50px;
  font-size: 1rem;
  border: 1px solid #00754a;
  transition: all 0.3s ease;
  
  &:hover {
    background-color: rgba(0, 117, 74, 0.1);
    transform: translateY(-2px);
  }
`;

const CategoriesSection = styled.section`
  padding: 64px 20px;
`;

const SectionTitle = styled.h2`
  font-size: 2rem;
  margin-bottom: 48px;
  text-align: center;
  
  @media (max-width: 768px) {
    font-size: 1.8rem;
  }
`;

const GiftCardGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 24px;
  max-width: 1200px;
  margin: 0 auto;
  
  @media (max-width: 576px) {
    grid-template-columns: repeat(2, 1fr);
    gap: 16px;
  }
`;

const GiftCardCategory = styled.div`
  text-align: center;
  cursor: pointer;
  transition: transform 0.3s ease;
  
  &:hover {
    transform: translateY(-5px);
  }
`;

const CategoryImage = styled.div`
  margin-bottom: 16px;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  
  img {
    width: 100%;
    display: block;
    transition: transform 0.3s ease;
  }
  
  &:hover img {
    transform: scale(1.05);
  }
`;

const CategoryName = styled.h3`
  font-size: 1.25rem;
  margin-bottom: 8px;
`;

const FeaturedSection = styled.section`
  padding: 64px 20px;
  background-color: #f7f7f7;
`;

const FeaturedGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 16px;
  max-width: 1200px;
  margin: 0 auto;
  
  @media (max-width: 576px) {
    grid-template-columns: repeat(2, 1fr);
  }
`;

const GiftCardItem = styled.div`
  cursor: pointer;
  transition: transform 0.3s ease;
  
  &:hover {
    transform: translateY(-5px);
  }
`;

const GiftCardImage = styled.div`
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  margin-bottom: 12px;
  
  img {
    width: 100%;
    display: block;
    transition: transform 0.3s ease;
  }
  
  &:hover img {
    transform: scale(1.05);
  }
`;

const GiftCardName = styled.h4`
  font-size: 1rem;
  text-align: center;
`;

const BulkSection = styled.section`
  padding: 64px 20px;
  text-align: center;
`;

const BulkContainer = styled.div`
  max-width: 800px;
  margin: 0 auto;
  background-color: #f2f0eb;
  padding: 48px;
  border-radius: 12px;
  
  @media (max-width: 768px) {
    padding: 32px 20px;
  }
`;

const BulkTitle = styled.h2`
  font-size: 2rem;
  margin-bottom: 24px;
  
  @media (max-width: 768px) {
    font-size: 1.8rem;
  }
`;

const BulkDescription = styled.p`
  color: rgba(0, 0, 0, 0.7);
  line-height: 1.7;
  font-size: 1.25rem;
  margin-bottom: 32px;
  
  @media (max-width: 768px) {
    font-size: 1rem;
  }
`;

// Mock data for categories
const categories = [
  {
    id: 'seasonal',
    name: 'Seasonal',
    image: 'https://globalassets.starbucks.com/assets/fc7d4cb9e4514258b98636550eba0442.jpg'
  },
  {
    id: 'birthday',
    name: 'Birthday',
    image: 'https://globalassets.starbucks.com/assets/025ef84d6a5a4dcd9fd3176dfcd23015.jpg'
  },
  {
    id: 'thank-you',
    name: 'Thank You',
    image: 'https://globalassets.starbucks.com/assets/a183d8bcac5942f2964d2c9f4e54a85a.jpg'
  },
  {
    id: 'celebration',
    name: 'Celebration',
    image: 'https://globalassets.starbucks.com/assets/0e0b95fd8c5442e6856f225b6a6b8e9b.jpg'
  }
];

// Mock data for featured gift cards
const featuredGiftCards = [
  {
    id: 1,
    name: 'Spring Flowers',
    image: 'https://globalassets.starbucks.com/assets/21ec2cb3dbc24950a28f966c9521ba85.jpg'
  },
  {
    id: 2,
    name: 'Thank You Heart',
    image: 'https://globalassets.starbucks.com/assets/4d33dff35a044dfcb41d094f95c1aef9.jpg'
  },
  {
    id: 3,
    name: 'Happy Birthday Cake',
    image: 'https://globalassets.starbucks.com/assets/45f1c4ed8dca429f9e37b1c7e1a1bd6d.jpg'
  },
  {
    id: 4,
    name: 'You Rock',
    image: 'https://globalassets.starbucks.com/assets/2d32db3489704581a47fb2b5d1d1171c.jpg'
  },
  {
    id: 5,
    name: 'Celebration Balloons',
    image: 'https://globalassets.starbucks.com/assets/8c34f576af314654956200e08429e9bd.jpg'
  },
  {
    id: 6,
    name: 'Summer Sunset',
    image: 'https://globalassets.starbucks.com/assets/7ad856a8cbdd4fbeb0119695a8d5b843.jpg'
  },
  {
    id: 7,
    name: 'Congratulations',
    image: 'https://globalassets.starbucks.com/assets/34d0b33ff387421b86e90b6bf8ef6bba.jpg'
  },
  {
    id: 8,
    name: 'Classic Siren',
    image: 'https://globalassets.starbucks.com/assets/45f1c4ed8dca429f9e37b1c7e1a1bd6d.jpg'
  }
];

function GiftCards() {
  return (
    <GiftCardsContainer>
      <HeroSection>
        <HeroContent>
          <HeroTitle>Gift Cards</HeroTitle>
          <HeroSubtitle>Say thanks with a Starbucks eGift. Fast, easy, and delicious!</HeroSubtitle>
          <ButtonContainer>
            <PrimaryButton to="/gift-cards/egift">Send an eGift</PrimaryButton>
            <SecondaryButton to="/gift-cards/redeem">Redeem a Gift</SecondaryButton>
          </ButtonContainer>
        </HeroContent>
      </HeroSection>
      
      <CategoriesSection>
        <SectionTitle>Gift Card Categories</SectionTitle>
        <GiftCardGrid>
          {categories.map(category => (
            <GiftCardCategory key={category.id}>
              <CategoryImage>
                <img src={category.image} alt={category.name} />
              </CategoryImage>
              <CategoryName>{category.name}</CategoryName>
            </GiftCardCategory>
          ))}
        </GiftCardGrid>
      </CategoriesSection>
      
      <FeaturedSection>
        <SectionTitle>Featured eGift Cards</SectionTitle>
        <FeaturedGrid>
          {featuredGiftCards.map(card => (
            <GiftCardItem key={card.id}>
              <GiftCardImage>
                <img src={card.image} alt={card.name} />
              </GiftCardImage>
              <GiftCardName>{card.name}</GiftCardName>
            </GiftCardItem>
          ))}
        </FeaturedGrid>
      </FeaturedSection>
      
      <BulkSection>
        <BulkContainer>
          <BulkTitle>Gift Cards in Bulk</BulkTitle>
          <BulkDescription>
            Starbucks Cards in bulk are an easy way to gift, reward, incentivize, or show appreciation. Gift Cards bulk orders of $500-$2,000 are available at a 5% discount.
          </BulkDescription>
          <PrimaryButton to="/gift-cards/corporate">Shop Corporate Gifts</PrimaryButton>
        </BulkContainer>
      </BulkSection>
    </GiftCardsContainer>
  );
}

export default GiftCards; 